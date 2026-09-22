import { TestBed } from '@angular/core/testing';
import { App } from './app';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App],
    })
      .compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('shows the current single list', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const page = fixture.nativeElement as HTMLElement;

    expect(page.querySelector('h1')?.textContent).toContain('ShopMate');
    expect(page.querySelectorAll('app-shopping-item')).toHaveLength(3);
  });

  it('frames the list with a main area and footer', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();
    const page = fixture.nativeElement as HTMLElement;

    expect(page.querySelector('main')).not.toBeNull();
    expect(page.querySelector('app-shop-footer')).not.toBeNull();
  });

  it('toggles only the requested item', async () => {
    const fixture = TestBed.createComponent(App);
    await fixture.whenStable();

    fixture.componentInstance.togglePurchased(2);

    expect(fixture.componentInstance.items.find((item) => item.id === 2)?.purchased).toBe(true);
    expect(fixture.componentInstance.items.find((item) => item.id === 1)?.purchased).toBe(false);
  });
});
