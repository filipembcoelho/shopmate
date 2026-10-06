import { TestBed } from '@angular/core/testing';
import { SessionDemo } from './session-demo';

describe('SessionDemo', () => {
  let service: SessionDemo;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SessionDemo);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
