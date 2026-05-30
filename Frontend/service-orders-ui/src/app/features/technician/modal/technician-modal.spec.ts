import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnicianModal } from './technician-modal';

describe('TechnicianModal', () => {
  let component: TechnicianModal;
  let fixture: ComponentFixture<TechnicianModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TechnicianModal],
    }).compileComponents();

    fixture = TestBed.createComponent(TechnicianModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
