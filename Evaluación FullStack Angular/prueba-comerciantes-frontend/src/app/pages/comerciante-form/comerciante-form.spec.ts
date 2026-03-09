import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComercianteForm } from './comerciante-form';

describe('ComercianteForm', () => {
  let component: ComercianteForm;
  let fixture: ComponentFixture<ComercianteForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComercianteForm],
    }).compileComponents();

    fixture = TestBed.createComponent(ComercianteForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
