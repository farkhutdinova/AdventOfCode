from utils import getInput, getTest

digit_names = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"]

def part1(lines):
    sum = 0
    for line in lines:
        digits = [char for char in line if char. isnumeric()]
        sum += int(digits[0]+digits[-1])
    return sum

def part2(lines):
    sum = 0
    for line in lines:
        digits = [char for char in translate(line) if char. isnumeric()]
        sum += int(digits[0]+digits[-1])
    return sum

def translate(line):
    for num, name in enumerate(digit_names):
        line = line.replace(name, f"{name}{num}{name}")
    return line

# part1
tlines = getTest(1,1)
testres = part1(tlines)
print(testres, testres == 142)

lines = getInput(1,1)
print(part1(lines))

# part2
tlines = getTest(1,2)
testres = part2(tlines)
print(testres, testres == 281)

lines = getInput(1,2)
print(part2(lines))