pub fn part1(input: &str) -> i32 {
    let mut max_calories = 0;
    for group in input.split("\r\n\r\n") {
        let mut calories = 0;
        for line in group.lines() {
            calories += line.trim().parse::<i32>().unwrap();
        }
        max_calories = max_calories.max(calories);
    }
    max_calories
}

pub fn part2(input: &str) -> i32 {
    let mut list_group: Vec<_> = input
        .split("\r\n\r\n")
        .into_iter()
        .map(|x| x.lines().map(|l| l.parse::<i32>().unwrap()).sum())
        .collect();

    list_group.sort();

    list_group.iter().rev().take(3).sum()
}

#[cfg(test)]
mod tests {

    #[test]
    fn part1_example() {
        let value = super::part1(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Part1 - Elf carrying the most Calories: {value}");
        assert_eq!(value, 24000);
    }

    #[test]
    fn part1_puzzle() {
        let value = super::part1(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Part1 - Elf carrying the most Calories: {value}");
        assert_eq!(value, 71780);
    }

    #[test]
    fn part2_example() {
        let value = super::part2(&std::fs::read_to_string("input/example.txt").unwrap());
        println!(
            "Part2 - Calories carried by the top three Elves carrying the most Calories: {value}"
        );
        assert_eq!(value, 45000);
    }

    #[test]
    fn part2_puzzle() {
        let value = super::part2(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!(
            "Part2 - Calories carried by the top three Elves carrying the most Calories: {value}"
        );
        assert_eq!(value, 212489);
    }
}
