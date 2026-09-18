let number = 12;
number = 15;

const pi = 3.14;

console.log('Hello World! The value of number is: ' + number);

const items = ['apple', 'banana', 'orange'];

items.push('grape');

items[3] = 'kiwi';

console.log('Items in the array: ' + items.join(', '));

const person = {
  name: 'John',
  age: 30,
  city: 'New York',
};

console.log('Person details: ' + person.name + ', ' + person.age + ', ' + person.city);

person.country = 'USA';

function addItemsToArray(newItem) {
  items.push(newItem);
}

console.log('Items before adding new item: ' + items.join(', '));

addItemsToArray('mango');

console.log('Items after adding new item: ' + items.join(', '));