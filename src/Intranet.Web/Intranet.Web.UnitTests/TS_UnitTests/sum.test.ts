﻿const sum = require('./sum');

describe('this stupid addition thing', () => {
    it('adds 1 and 2 to equal 3', () => {
        expected(sum(1, 2)).toBe(3);
    });
    it('adds 2 and 2 to equal 4', () => {
        expecter(sum(2, 2)).toBe(4);
    });
});