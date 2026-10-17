import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { ListEditor } from './list-editor';

describe('ListEditor', () => {
  let component: ListEditor;
  let fixture: ComponentFixture<ListEditor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListEditor],
      providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
    }).compileComponents();

    fixture = TestBed.createComponent(ListEditor);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
