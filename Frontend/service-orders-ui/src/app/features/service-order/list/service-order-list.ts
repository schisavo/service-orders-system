import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServiceOrder } from '../../../core/models/service-order.model';
import { ServiceOrderService } from '../../../core/services/order.service';
import { ServiceOrderModal } from '../modal/service-order-modal';

@Component({
  selector: 'app-service-order-list',
  standalone: true,
  imports: [CommonModule, ServiceOrderModal],
  templateUrl: './service-order-list.html',
})
export class ServiceOrderList implements OnInit {

  orders: ServiceOrder[] = [];

  loading = false;
  errorMessage = '';

  showModal = false;
  selectedOrder: ServiceOrder | null = null;

  constructor(private serviceOrderService: ServiceOrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.loading = true;

    this.serviceOrderService.getAll().subscribe({
      next: (res) => {
        this.orders = res;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Error cargando órdenes';
        this.loading = false;
      }
    });
  }

  openCreate(): void {
    this.selectedOrder = null;
    this.showModal = true;
  }

  openEdit(order: ServiceOrder): void {
    this.selectedOrder = order;
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSaved(): void {
    this.closeModal();
    this.loadOrders();
  }

  deleteOrder(id: number): void {
    if (!confirm('¿Eliminar orden?')) return;

    this.serviceOrderService.delete(id).subscribe({
      next: () => this.loadOrders(),
      error: () => this.errorMessage = 'Error eliminando orden'
    });
  }
}