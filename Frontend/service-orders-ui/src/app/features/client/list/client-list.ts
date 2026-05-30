import { ChangeDetectorRef, Component } from '@angular/core';
import { ClientService } from '../../../core/services/client.service';
import { Client } from '../../../core/models/client.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-client-list',
  imports: [CommonModule],
  templateUrl: './client-list.html',
  styleUrl: './client-list.css',
})
export class ClientList {
clients: Client[] = [];
  errorMessage = '';
  loading = false;

  constructor(private clientService: ClientService,
  private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadClients();
  }

  loadClients() {
    this.loading = true;

    this.clientService.getAll().subscribe({
      next: (res) => {
        this.clients = res;
        this.loading = false;

        this.cdr.detectChanges();

        console.log('Respuesta:', res);
        console.log('Loading:', this.loading);
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

