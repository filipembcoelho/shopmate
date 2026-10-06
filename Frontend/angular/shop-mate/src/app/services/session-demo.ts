import { Service } from '@angular/core';
import { of, map, filter, delay } from 'rxjs';

@Service()
export class SessionDemoService {
  run() {
    // this.namedMethod();
    // this.arrowFunctionMethod();
    // this.forEachDemo();
    // this.arrayMethodsDemo();
    this.observableDemo();
  }

  private namedMethod() {
    this.showItem3('Milk');
  }

  private arrowFunctionMethod() {
    const showItem2 = (item: string) => {
      console.log(item);
    };

    showItem2('Eggs');
  }

  private forEachDemo() {
    const items = ['Bread', 'Butter', 'Cheese'];

    items.forEach((item) => {
      this.showItem(item);
    });
  }

  readonly showItem3 = (item: string) => {
    console.log(item);
  };

  private showItem(item: string): void {
    console.log(item);
  }

  private arrayMethodsDemo() {
    const items = [
      { name: 'Bread', purchased: true },
      { name: 'Butter', purchased: false },
      { name: 'Cheese', purchased: true },
      { name: 'Milk', purchased: false },
    ];

    const labels = items
      .filter((item) => !item.purchased)
      .map((item) => {
        return 'Buy ' + item.name;
      });

    // items.forEach((item) => {
    //   if (!item.purchased) {
    //     console.log('Buy ' + item.name);
    //   }
    // });

    console.log(labels);
  }

  private observableDemo() {
    of('Bread', 'Butter', 'Cheese')
      .pipe(
        delay(10000),
        map((item) => {
          return 'Buy ' + item;
        }),
        filter((item) => {
          return !item.includes('Butter');
        }),
      )
      .subscribe((item) => {
        console.log(item);
      });
  }
}
