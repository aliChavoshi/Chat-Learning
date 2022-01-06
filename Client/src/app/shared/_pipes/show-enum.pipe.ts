import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'showEnum',
})
export class ShowEnumPipe implements PipeTransform {
  transform(value: any): any {
    let myEnum = [];
    let objectEnum = Object.keys(value);
    const values = objectEnum.slice(0, objectEnum.length / 2);
    const keys = objectEnum.slice(objectEnum.length / 2);

    for (let i = 0; i < objectEnum.length / 2; i++) {
      myEnum.push({ value: keys[i], key: values[i] });
    }
    return myEnum;
  }
}
