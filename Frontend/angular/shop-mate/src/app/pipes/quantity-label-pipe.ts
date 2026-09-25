import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'quantityLabel',
})
export class QuantityLabelPipe implements PipeTransform {
  transform(value: number): string {
    return value === 1 ? '1 unit' : `${value} units`;
  }
}
