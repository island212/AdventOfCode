static TABLE_SCORE: [i32; 128] = [
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
    0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x00, 0x00, 0x00, 0x00, 0x00,
];

pub fn part1(input: &str) -> i32 {
    let mut table: [i32; 128] = [0; 128];

    let mut score = 0;
    for line in input.lines() {
        let (left, right) = line.split_at(line.len() / 2);
        for c in left.as_bytes() {
            table[*c as usize] += 1;
        }
        for c in right.as_bytes() {
            if table[*c as usize] > 0 {
                score += TABLE_SCORE[*c as usize];
                break;
            }
        }
        for c in line.as_bytes() {
            table[*c as usize] = 0;
        }
    }
    score
}

pub fn part2(input: &str) -> i32 {
    let mut table1: [i32; 128] = [0; 128];
    let mut table2: [i32; 128] = [0; 128];

    let mut score = 0;
    let lines: Vec<_> = input.lines().collect();

    for x in (0..lines.len()).step_by(3) {
        for c in lines[x].as_bytes() {
            table1[*c as usize] = 1;
        }
        for c in lines[x + 1].as_bytes() {
            table2[*c as usize] = 1;
        }
        for c in lines[x + 2].as_bytes() {
            if table1[*c as usize] == 1 && table2[*c as usize] == 1 {
                score += TABLE_SCORE[*c as usize];
                break;
            }
        }
        for elem in table1.iter_mut() {
            *elem = 0;
        }
        for elem in table2.iter_mut() {
            *elem = 0;
        }
    }
    score
}

#[cfg(test)]
mod tests {

    #[test]
    fn part1_example() {
        let value = super::part1(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part1 - {value}");
        assert_eq!(value, 157);
    }

    #[test]
    fn part1_puzzle() {
        let value = super::part1(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part1 - {value}");
        assert_eq!(value, 8185);
    }

    #[test]
    fn part2_example() {
        let value = super::part2(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part2 - {value}");
        assert_eq!(value, 70);
    }

    #[test]
    fn part2_puzzle() {
        let value = super::part2(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part2 - {value}");
        assert_eq!(value, 2817);
    }
}
