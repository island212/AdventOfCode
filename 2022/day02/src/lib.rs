pub fn part1(input: &str) -> i32 {
    let mut score = 0;
    for line in input.lines() {
        let split = line.split(' ').collect::<Vec<&str>>();
        score += match split[0] {
            //Rock
            "A" => match split[1] {
                "X" => 1 + 3, // Rock
                "Y" => 2 + 6, // Paper
                "Z" => 3 + 0, // Scissors
                _ => panic!("invalid played_rock move"),
            },
            // Paper
            "B" => match split[1] {
                "X" => 1 + 0, // Rock
                "Y" => 2 + 3, // Paper
                "Z" => 3 + 6, // Scissors
                _ => panic!("invalid played_rock move"),
            },
            // Scissors
            "C" => match split[1] {
                "X" => 1 + 6, // Rock
                "Y" => 2 + 0, // Paper
                "Z" => 3 + 3, // Scissors
                _ => panic!("invalid played_rock move"),
            },
            _ => panic!("invalid play move"),
        };
    }
    score
}

pub fn part2(input: &str) -> i32 {
    let mut score = 0;
    for line in input.lines() {
        let split = line.split(' ').collect::<Vec<&str>>();
        score += match split[0] {
            //Rock
            "A" => match split[1] {
                "X" => 3 + 0, // Scissors
                "Y" => 1 + 3, // Rock
                "Z" => 2 + 6, // Paper
                _ => panic!("invalid played_rock move"),
            },
            // Paper
            "B" => match split[1] {
                "X" => 1 + 0, // Rock
                "Y" => 2 + 3, // Paper
                "Z" => 3 + 6, // Scissors
                _ => panic!("invalid played_rock move"),
            },
            // Scissors
            "C" => match split[1] {
                "X" => 2 + 0, // Paper
                "Y" => 3 + 3, // Scissors
                "Z" => 1 + 6, // Rock
                _ => panic!("invalid played_rock move"),
            },
            _ => panic!("invalid play move"),
        };
    }
    score
}

#[cfg(test)]
mod tests {

    #[test]
    fn part1_example() {
        let value = super::part1(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part1 - {value}");
        assert_eq!(value, 15);
    }

    #[test]
    fn part1_puzzle() {
        let value = super::part1(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part1 - {value}");
        assert_eq!(value, 11767);
    }

    #[test]
    fn part2_example() {
        let value = super::part2(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part2 - {value}");
        assert_eq!(value, 12);
    }

    #[test]
    fn part2_puzzle() {
        let value = super::part2(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part2 - {value}");
        assert_eq!(value, 13886);
    }
}
