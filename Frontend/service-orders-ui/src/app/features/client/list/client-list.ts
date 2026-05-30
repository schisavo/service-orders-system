import { ChangeDetectorRef, Component } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientService } from '../../../core/services/client.service';
import { Client } from '../../../core/models/client.model';

import { ClientModal } from '../modal/client-modal';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [
    CommonModule,
    ClientModal
  ],
  templateUrl: './client-list.html',
  styleUrl: './client-list.css',
})
export class ClientList {
  clients: Client[] = [];
  loading = false;
  errorMessage = '';
  showModal = false;
  selectedClient: Client | null = null;

  constructor(
    private clientService: ClientService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadClients();
  }

  loadClients() {
    this.loading = true;
    this.clientService.getAll().subscribe({
      next: (res) => {
        console.log('CLIENTES NUEVOS', res);
        this.clients = [...res];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  openCreateModal() {
    this.selectedClient = null;
    this.showModal = true;
  }

  openEditModal(client: Client) {
    this.selectedClient = client;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  onSaved() {
    this.closeModal();
    this.loadClients();
  }

  deleteClient(id: number) {
    if (!confirm('¿Eliminar cliente?')) {
      return;
    }
    this.clientService.delete(id).subscribe({
      next: () => this.loadClients(),
      error: () => {
        this.errorMessage = 'Error al eliminar cliente';
      }
    });
  }
}