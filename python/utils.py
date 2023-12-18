def getTest(day, task):
    f = open(f"2023/day{day}/test{task}.txt", 'r')
    input = f.read().strip()
    return input.splitlines()

def getInput(day, task):
    f = open(f"2023/day{day}/input{task}.txt", 'r')
    input = f.read().strip()
    return input.splitlines()