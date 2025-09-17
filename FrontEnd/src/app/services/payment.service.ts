import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'http://localhost:5174/api/Payment'; // Replace with your backend URL

  constructor(private http: HttpClient) { }

  getRental(rentalId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/rental/${rentalId}`);
  }

  addPayment(payment: any): Observable<any> {
    return this.http.post(this.apiUrl, payment);
  }

  getPaymentById(paymentId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${paymentId}`);
  }
}
