import { TestBed } from '@angular/core/testing';

import { Comerciante } from './comerciante';

describe('Comerciante', () => {
  let service: Comerciante;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Comerciante);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
