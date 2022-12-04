fn get_assignements(input: &str) -> Vec<i32> {
    input
        .split("-")
        .map(|x| x.parse::<i32>().unwrap())
        .collect()
}

pub fn part1(input: &str) -> i32 {
    let mut count = 0;
    for line in input.lines() {
        let pairs = line.split(",").collect::<Vec<&str>>();
        let assign1 = get_assignements(pairs[0]);
        let assign2 = get_assignements(pairs[1]);
        if (assign1[0] <= assign2[0] && assign2[1] <= assign1[1])
            || (assign2[0] <= assign1[0] && assign1[1] <= assign2[1])
        {
            count += 1;
        }
    }
    count
}

pub fn part2(input: &str) -> i32 {
    let mut count = 0;
    for line in input.lines() {
        let pairs = line.split(",").collect::<Vec<&str>>();
        let assign1 = get_assignements(pairs[0]);
        let assign2 = get_assignements(pairs[1]);
        if !(assign1[1] < assign2[0]
            || assign1[0] > assign2[1]
            || assign2[1] < assign1[0]
            || assign2[0] > assign1[1])
        {
            count += 1;
        }
    }
    count
}

#[cfg(test)]
mod tests {

    #[test]
    fn part1_example() {
        let value = super::part1(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part1 - {value}");
        assert_eq!(value, 2);
    }

    #[test]
    fn part1_puzzle() {
        let value = super::part1(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part1 - {value}");
        assert_eq!(value, 487);
    }

    #[test]
    fn part2_example() {
        let value = super::part2(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part2 - {value}");
        assert_eq!(value, 4);
    }

    #[test]
    fn part2_puzzle() {
        let value = super::part2(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part2 - {value}");
        assert_eq!(value, 849);
    }
}
