use std::iter;

fn fill_cargos(input: &str) -> Vec<Vec<char>> {
    let cargo_lines = input.lines().collect::<Vec<&str>>();
    let stack_count = cargo_lines[cargo_lines.len() - 1].split("   ").count();

    let mut cargos: Vec<_> = iter::repeat_with(|| Vec::<char>::new())
        .take(stack_count)
        .collect();

    for cargo_index in (0..cargo_lines.len() - 1).rev() {
        let c_lines = cargo_lines[cargo_index].chars().collect::<Vec<_>>();
        for x in 0..stack_count {
            let c = c_lines[x * 4 + 1];
            if c != ' ' {
                cargos[x].push(c);
                println!("{x} -> {c}");
            }
        }
        println!("");
    }
    println!("");
    cargos
}

pub fn part1(input: &str) -> String {
    let mut part = input.split("\r\n\r\n");
    let mut cargos = fill_cargos(part.next().unwrap());

    for procedure_line in part.next().unwrap().lines() {
        let procedure_steps = procedure_line.split_ascii_whitespace().collect::<Vec<_>>();

        let quantity = procedure_steps[1].parse::<usize>().unwrap();
        let source = procedure_steps[3].parse::<usize>().unwrap() - 1;
        let destination = procedure_steps[5].parse::<usize>().unwrap() - 1;

        for _ in 0..quantity {
            let source_value = cargos[source].pop().unwrap();
            cargos[destination].push(source_value);
        }
    }

    let mut result = String::new();
    for stack in cargos {
        result.push(*stack.last().unwrap());
    }
    result
}

pub fn part2(input: &str) -> String {
    let mut part = input.split("\r\n\r\n");
    let mut cargos = fill_cargos(part.next().unwrap());

    let mut temp_swap = Vec::<char>::new();
    for procedure_line in part.next().unwrap().lines() {
        let procedure_steps = procedure_line.split_ascii_whitespace().collect::<Vec<_>>();

        let quantity = procedure_steps[1].parse::<usize>().unwrap();
        let source = procedure_steps[3].parse::<usize>().unwrap() - 1;
        let destination = procedure_steps[5].parse::<usize>().unwrap() - 1;

        for _ in 0..quantity {
            let source_value = cargos[source].pop().unwrap();
            temp_swap.push(source_value);
        }
        for _ in 0..quantity {
            let destination_value = temp_swap.pop().unwrap();
            cargos[destination].push(destination_value);
        }
        temp_swap.clear();
    }

    let mut result = String::new();
    for stack in cargos {
        result.push(*stack.last().unwrap());
    }
    result
}

#[cfg(test)]
mod tests {

    #[test]
    fn part1_example() {
        let value = super::part1(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part1 - {value}");
        assert_eq!(value, "CMZ");
    }

    #[test]
    fn part1_puzzle() {
        let value = super::part1(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part1 - {value}");
        assert_eq!(value, "GRTSWNJHH");
    }

    #[test]
    fn part2_example() {
        let value = super::part2(&std::fs::read_to_string("input/example.txt").unwrap());
        println!("Example Part2 - {value}");
        assert_eq!(value, "MCD");
    }

    #[test]
    fn part2_puzzle() {
        let value = super::part2(&std::fs::read_to_string("input/puzzle.txt").unwrap());
        println!("Puzzle Part2 - {value}");
        assert_eq!(value, "QLFQDBBHM");
    }
}
