import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Client } from '../../../core/models/client.model';
import { ClientService } from '../../../core/services/client.service';

@Component({
  selector: 'app-client-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './client-form.html',
  styleUrl: './client-form.css',
})
export class ClientForm {
  client: Client = {
    id: 0,
    fullName: '',
    documentNumber: '',
    address: '',
    phone: ''
  };

  constructor(private clientService: ClientService) {}

  save() {
    if(this.client.id === 0) {
      this.clientService.create(this.client).subscribe({
        next: () => alert('Cliente creado'),
        error: () => alert('Error al crear cliente')
      });
    } else {
      this.clientService.update(this.client.id, this.client).subscribe({
        next: () => alert('Cliente actualizado'),
        error: () => alert('Error al actualizar cliente')
      });
    }
  }
}
