import { Component } from '@angular/core';
import { PaymentService } from '../services/payment.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent {
  rentalId!: number;
  rental: any;
  payment: any;

  constructor(private paymentService: PaymentService) {}

  fetchRental() {
    this.paymentService.getRental(this.rentalId).subscribe({
      next: (data) => this.rental = data,
      error: (err) => alert(err.error.message || 'Error fetching rental')
    });
  }

  pay() {
    if (!this.rental || !this.rental.rentalID) return;

    const paymentInput = { rentalID: this.rental.rentalID };

    this.paymentService.addPayment(paymentInput).subscribe({
      next: (data) => this.payment = data,
      error: (err) => alert(err.error.message || 'Payment failed')
    });
  }
}
