import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'quantityLabel',
})
export class QuantityLabelPipe implements PipeTransform {
  transform(value: number): unknown {
    return value === 1 ? '1 unit' : `${value} units`;
  }
}

// $"{name} is his name!"
