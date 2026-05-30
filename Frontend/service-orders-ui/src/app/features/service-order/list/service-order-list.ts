import { Component } from '@angular/core';
import { ServiceOrder } from '../../../core/models/service-order.model';
import { ServiceOrderService } from '../../../core/services/order.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-service-order-list',
  imports: [CommonModule],
  templateUrl: './service-order-list.html',
  styleUrl: './service-order-list.css',
})
export class ServiceOrderList {
  orders: ServiceOrder[] = [];
  errorMessage = '';
  loading = false;

  constructor(private serviceOrderService: ServiceOrderService) {}

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.loading = true;
    this.serviceOrderService.getAll().subscribe({
      next: (res) => {
        this.orders = res;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Error al cargar órdenes';
        this.loading = false;
      }
    });
  }

  deleteOrder(id:number) {
    if(confirm('¿Eliminar orden de servicio?')) {
      this.serviceOrderService.delete(id).subscribe({
        next: () => this.loadOrders(),
        error: () => this.errorMessage = 'Error al eliminar orden'
      });
    }
  }

  getBadgeClass(status:string) {
    switch(status) {
      case 'Pendiente': return 'bg-yellow-200 text-yellow-800 px-2 py-1 rounded';
      case 'En progreso': return 'bg-blue-200 text-blue-800 px-2 py-1 rounded';
      case 'Finalizada': return 'bg-green-200 text-green-800 px-2 py-1 rounded';
      default: return 'bg-gray-200 text-gray-800 px-2 py-1 rounded';
    }
  }
}
