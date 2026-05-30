import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceOrderModal } from './service-order-modal';

describe('ServiceOrderModal', () => {
  let component: ServiceOrderModal;
  let fixture: ComponentFixture<ServiceOrderModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiceOrderModal],
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceOrderModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
