import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NuevoRegistroComponent } from './nuevo-registro.component';

describe('NuevoRegistroComponent', () => {
  let component: NuevoRegistroComponent;
  let fixture: ComponentFixture<NuevoRegistroComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NuevoRegistroComponent]
    });
    fixture = TestBed.createComponent(NuevoRegistroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
