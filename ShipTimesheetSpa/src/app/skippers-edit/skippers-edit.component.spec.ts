import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkippersEditComponent } from './skippers-edit.component';

describe('SkippersEditComponent', () => {
  let component: SkippersEditComponent;
  let fixture: ComponentFixture<SkippersEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkippersEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkippersEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
