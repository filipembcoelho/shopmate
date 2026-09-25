import { interval, map, take } from 'rxjs';

const stages = ['Order received', 'Packing', 'Out for delivery'];

console.log('Tracking a delivery...');

interval(1000)
  .pipe(
    take(stages.length),
    map((index) => stages[index]),
  )
  .subscribe({
    next: (status) => console.log(status),
    complete: () => console.log('Tracking finished'),
  });
