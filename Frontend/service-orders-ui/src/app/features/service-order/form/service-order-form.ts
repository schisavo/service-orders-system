import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ServiceOrder } from '../../../core/models/service-order.model';
import { ServiceOrderService } from '../../../core/services/order.service';

@Component({
  selector: 'app-service-order-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './service-order-form.html',
  styleUrl: './service-order-form.css',
})
export class ServiceOrderForm {

  order: ServiceOrder = {
    id: 0,
    createdAt: new Date().toISOString(),
    status: 'Pendiente',
    description: '',
    technicianId: 0,
    clientId: 0
  };

  constructor(private serviceOrderService: ServiceOrderService) {}

  save() {
    if(this.order.id === 0) {
      this.serviceOrderService.create(this.order).subscribe({
        next: () => alert('Orden creada'),
        error: () => alert('Error al crear orden')
      });
    } else {
      this.serviceOrderService.update(this.order.id, this.order).subscribe({
        next: () => alert('Orden actualizada'),
        error: () => alert('Error al actualizar orden')
      });
    }
  }
}