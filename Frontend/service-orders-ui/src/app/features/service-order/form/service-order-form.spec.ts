import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceOrderForm } from './service-order-form';

describe('ServiceOrderForm', () => {
  let component: ServiceOrderForm;
  let fixture: ComponentFixture<ServiceOrderForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiceOrderForm],
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceOrderForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
