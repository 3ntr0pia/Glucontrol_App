import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrosAnterioresComponent } from './registros-anteriores.component';

describe('RegistrosAnterioresComponent', () => {
  let component: RegistrosAnterioresComponent;
  let fixture: ComponentFixture<RegistrosAnterioresComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RegistrosAnterioresComponent]
    });
    fixture = TestBed.createComponent(RegistrosAnterioresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
