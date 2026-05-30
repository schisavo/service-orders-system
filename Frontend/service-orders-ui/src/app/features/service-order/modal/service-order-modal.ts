import {
  Component,
  EventEmitter,
  Input,
  Output,
  OnInit
} from '@angular/core';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { CommonModule } from '@angular/common';
import { forkJoin } from 'rxjs';

import { ServiceOrder } from '../../../core/models/service-order.model';
import { Technician } from '../../../core/models/technician.model';
import { Client } from '../../../core/models/client.model';

import { ServiceOrderService } from '../../../core/services/order.service';
import { TechnicianService } from '../../../core/services/technician.service';
import { ClientService } from '../../../core/services/client.service';

@Component({
  selector: 'app-service-order-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './service-order-modal.html',
})
export class ServiceOrderModal implements OnInit {

  @Input() order: ServiceOrder | null = null;

  @Output() close = new EventEmitter<void>();
  @Output() saved = new EventEmitter<void>();

  orderForm!: FormGroup;

  technicians: Technician[] = [];
  clients: Client[] = [];

  loading = false;

  constructor(
    private fb: FormBuilder,
    private serviceOrderService: ServiceOrderService,
    private technicianService: TechnicianService,
    private clientService: ClientService
  ) {}

  ngOnInit(): void {

    this.orderForm = this.fb.group({
      status: ['Pendiente', Validators.required],
      description: ['', Validators.required],
      technicianId: [null, Validators.required],
      clientId: [null, Validators.required]
    });

    this.loadCatalogs();

    if (this.order) {
      this.orderForm.patchValue({
        status: this.order.status,
        description: this.order.description,
        technicianId: this.order.technicianId,
        clientId: this.order.clientId
      });
    }
  }

  get isEditMode(): boolean {
    return !!this.order;
  }

  loadCatalogs(): void {
    this.loading = true;

    forkJoin({
      technicians: this.technicianService.getAll(),
      clients: this.clientService.getAll()
    }).subscribe({
      next: (res) => {
        this.technicians = res.technicians;
        this.clients = res.clients;
        this.loading = false;
      },
      error: () => this.loading = false
    });
  }

  save(): void {

    if (this.orderForm.invalid) return;

    const payload = this.orderForm.value;

    if (this.isEditMode && this.order) {

      this.serviceOrderService.update(this.order.id, {
        id: this.order.id,
        createdAt: this.order.createdAt,
        ...payload
      }).subscribe(() => {
        this.saved.emit();
        this.close.emit();
      });

    } else {

      this.serviceOrderService.create(payload).subscribe(() => {
        this.saved.emit();
        this.close.emit();
      });
    }
  }

  closeModal(): void {
    this.close.emit();
  }
}