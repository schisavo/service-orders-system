import { Component } from '@angular/core';
import { ClientService } from '../../../core/services/client.service';
import { Client } from '../../../core/models/client.model';

@Component({
  selector: 'app-client-list',
  imports: [],
  templateUrl: './client-list.html',
  styleUrl: './client-list.css',
})
export class ClientList {
clients: Client[] = [];
  errorMessage = '';
  loading = false;

  constructor(private clientService: ClientService) {}

  ngOnInit() {
    this.loadClients();
  }

  loadClients() {
    this.loading = true;
    this.clientService.getAll().subscribe({
      next: (res) => {
        this.clients = res;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Error al cargar clientes';
        this.loading = false;
      }
    });
  }

  deleteClient(id:number) {
    if(confirm('¿Eliminar cliente?')) {
      this.clientService.delete(id).subscribe({
        next: () => this.loadClients(),
        error: () => this.errorMessage = 'Error al eliminar cliente'
      });
    }
  }
}

