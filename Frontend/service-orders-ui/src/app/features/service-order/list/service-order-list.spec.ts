import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceOrderList } from './service-order-list';

describe('ServiceOrderList', () => {
  let component: ServiceOrderList;
  let fixture: ComponentFixture<ServiceOrderList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ServiceOrderList],
    }).compileComponents();

    fixture = TestBed.createComponent(ServiceOrderList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
