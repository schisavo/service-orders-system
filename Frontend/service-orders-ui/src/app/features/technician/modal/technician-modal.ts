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

import { Technician } from '../../../core/models/technician.model';
import { TechnicianService } from '../../../core/services/technician.service';

@Component({
  selector: 'app-technician-modal',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './technician-modal.html',
  styleUrl: './technician-modal.css'
})
export class TechnicianModal implements OnInit {

  @Input()
  technician?: Technician | null;

  @Output()
  close = new EventEmitter<void>();

  @Output()
  saved = new EventEmitter<void>();

  technicianForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private technicianService: TechnicianService
  ) {}

  ngOnInit() {

    this.technicianForm = this.fb.group({
      fullName: ['', Validators.required],
      phone: ['', Validators.required],
      specialty: ['', Validators.required]
    });

    if (this.technician) {

      this.technicianForm.patchValue({
        fullName: this.technician.fullName,
        phone: this.technician.phone,
        specialty: this.technician.specialty
      });

    }
  }

  get isEditMode(): boolean {
    return !!this.technician;
  }

  saveTechnician() {

    if (this.technicianForm.invalid) {
      this.technicianForm.markAllAsTouched();
      return;
    }

    const payload = this.technicianForm.value;

    if (this.isEditMode) {

      this.technicianService.update(
        this.technician!.id,
        {
          id: this.technician!.id,
          ...payload
        }
      ).subscribe(() => {

        this.saved.emit();
        this.close.emit();

      });

    } else {

      this.technicianService.create(payload)
      .subscribe(() => {

        this.saved.emit();
        this.close.emit();

      });

    }
  }

  closeModal() {
    this.close.emit();
  }
}