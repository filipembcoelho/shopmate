import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ListEditor } from './list-editor';

describe('ListEditor', () => {
  let component: ListEditor;
  let fixture: ComponentFixture<ListEditor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListEditor],
    }).compileComponents();

    fixture = TestBed.createComponent(ListEditor);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
