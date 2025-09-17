using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Step 1: Get rental details before payment
        [HttpGet("rental/{rentalId}")]
        public IActionResult GetRental(int rentalId)
        {
            var rental = _dbContext.Rentals
                .Include(r => r.Movie)
                .FirstOrDefault(r => r.RentalID == rentalId);

            if (rental == null)
                return NotFound(new { message = "Rental not found." });

            return Ok(new
            {
                rental.RentalID,
                rental.RentalDate,
                rental.ReturnDate,
                MovieTitle = rental.Movie?.Title,
                MovieBluRayCost = rental.Movie?.BluRayCost
            });
        }

        // Step 2: Add payment after movie is returned
        [HttpPost]
        public IActionResult AddPayment([FromBody] Payment paymentInput)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rental = _dbContext.Rentals
                .Include(r => r.Movie)
                .FirstOrDefault(r => r.RentalID == paymentInput.RentalID);

            if (rental == null)
                return NotFound(new { message = "Rental not found." });

            if (rental.Movie == null)
                return NotFound(new { message = "Movie not found for this rental." });

            if (rental.ReturnDate == null)
                return BadRequest(new { message = "Movie has not been returned yet. Payment cannot be processed." });

            decimal bluRayCost = rental.Movie.BluRayCost;

            // Calculate rental days (minimum 1 day)
            int rentalDays = (int)((rental.ReturnDate.Value - rental.RentalDate).TotalDays);
            rentalDays = rentalDays == 0 ? 1 : rentalDays;

            // Calculate amounts
            paymentInput.BaseAmount = Math.Round(bluRayCost * 0.10m * rentalDays, 2);
            paymentInput.GSTAmount = Math.Round(paymentInput.BaseAmount * 0.18m, 2);
            paymentInput.TotalAmount = Math.Round(paymentInput.BaseAmount + paymentInput.GSTAmount, 2);
            paymentInput.PaidOn = DateTime.Now;

            // Avoid EF Core tracking conflict
            paymentInput.Rental = null;

            _dbContext.Payments.Add(paymentInput);
            _dbContext.SaveChanges();

            // Return full bill (Payment + Rental + Movie)
            var fullBill = _dbContext.Payments
                .Include(p => p.Rental)
                    .ThenInclude(r => r.Movie)
                .FirstOrDefault(p => p.PaymentID == paymentInput.PaymentID);

            return CreatedAtAction(nameof(GetPaymentById), new { id = paymentInput.PaymentID }, fullBill);
        }

        // Step 3: Get payment (bill) by ID
        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _dbContext.Payments
                .Include(p => p.Rental)
                    .ThenInclude(r => r.Movie)
                .FirstOrDefault(p => p.PaymentID == id);

            if (payment == null)
                return NotFound(new { message = "Payment not found." });

            return Ok(payment);
        }
    }
}
