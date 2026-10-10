import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'quantityLabel',
})
export class QuantityLabelPipe implements PipeTransform {
  transform(value: number, unit?: number): string {
    if (unit === 2) return `${value} kg`;
    if (unit === 7) return value === 1 ? '1 package' : `${value} packages`;
    return value === 1 ? '1 unit' : `${value} units`;
  }
}
