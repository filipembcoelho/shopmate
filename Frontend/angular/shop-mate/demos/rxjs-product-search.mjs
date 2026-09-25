import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

const searchTerms = new Subject();

searchTerms
  .pipe(debounceTime(400), distinctUntilChanged())
  .subscribe((term) => console.log('Search for:', term));

function type(term) {
  console.log('Typed:', term);
  searchTerms.next(term);
}

type('m');
setTimeout(() => type('mi'), 100);
setTimeout(() => type('milk'), 200);
setTimeout(() => type('milk'), 800);
setTimeout(() => type('bread'), 1500);
setTimeout(() => searchTerms.complete(), 2200);
