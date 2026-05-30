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

import { ClientService } from '../../../core/services/client.service';
import { Client } from '../../../core/models/client.model';

@Component({
  selector: 'app-client-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './client-modal.html',
  styleUrl: './client-modal.css'
})
export class ClientModal implements OnInit {

  @Input() client?: Client | null;

  @Output() close = new EventEmitter<void>();

  @Output() saved = new EventEmitter<void>();

  clientForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private clientService: ClientService
  ) {}

  ngOnInit() {

    this.clientForm = this.fb.group({
      fullName: ['', Validators.required],
      documentNumber: ['', Validators.required],
      address: [''],
      phone: ['', Validators.required]
    });

    if (this.client) {

      this.clientForm.patchValue({
        fullName: this.client.fullName,
        documentNumber: this.client.documentNumber,
        address: this.client.address,
        phone: this.client.phone
      });
    }
  }

  get isEditMode(): boolean {
    return !!this.client;
  }

  saveClient() {
    if (this.clientForm.invalid) {
      this.clientForm.markAllAsTouched();
      return;
    }
    const payload = this.clientForm.value;
    if (this.isEditMode) {
      this.clientService
        .update(this.client!.id, {
          id: this.client!.id,
          ...payload
        })
        .subscribe({
          next: () => {
            this.saved.emit();
            this.close.emit();
          },
          error: (err) => {
            console.error(err);
          }
        });
    } else {
      this.clientService
        .create(payload)
        .subscribe({
          next: () => {
            this.saved.emit();
            this.close.emit();
          },
          error: (err) => {
            console.error(err);
          }
        });
    }
  }

  closeModal() {
    this.close.emit();
  }
}