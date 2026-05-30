import { ChangeDetectorRef, Component } from '@angular/core';
import { Technician } from '../../../core/models/technician.model';
import { TechnicianService } from '../../../core/services/technician.service';
import { CommonModule } from '@angular/common';
import { TechnicianModal } from '../modal/technician-modal';

@Component({
  selector: 'app-technician-list',
  imports: [
    CommonModule,
    TechnicianModal
  ],
  templateUrl: './technician-list.html',
  styleUrl: './technician-list.css',
})
export class TechnicianList {
  technicians: Technician[] = [];
  loading = false;
  errorMessage = '';
  showModal = false;
  selectedTechnician: Technician | null = null;

  constructor(
    private technicianService: TechnicianService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadTechnicians();
  }

  loadTechnicians() {
    this.loading = true;
    this.technicianService.getAll().subscribe({
      next: (res) => {
        console.log('TECNICOS NUEVOS', res);
        this.technicians = [...res];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  openCreateModal() {
    this.selectedTechnician = null;
    this.showModal = true;
  }

  openEditModal(technician: Technician) {
    this.selectedTechnician = technician;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  onSaved() {
    this.closeModal();
    this.loadTechnicians();
  }

  deleteTechnicians(id: number) {
    if (!confirm('¿Eliminar tecnico?')) {
      return;
    }
    this.technicianService.delete(id).subscribe({
      next: () => this.loadTechnicians(),
      error: () => {
        this.errorMessage = 'Error al eliminar tecnico';
      }
    });
  }
}