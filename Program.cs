
class TreeOp
{
    public long N {get; set;}
    public string Name{get; set;}

    public TreeOp Parent {get; set;}
    public TreeOp Left{get; set;}
    public TreeOp Right{get; set;}

    public char Op { get; set; }

    public TreeOp(string name)
    {
        Name = name;
    }

    public long Shout()
    {
        switch (Op)
        {
            case 's': return N;
            case '+': return Left.Shout() + Right.Shout();
            case '-': return Left.Shout() - Right.Shout();
            case '*': return Left.Shout() * Right.Shout();
            case '/': return Left.Shout() / Right.Shout();
        }

        return -1;
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Dictionary<string, TreeOp> monkeys = new Dictionary<string, TreeOp>();
        string[] lines = File.ReadAllLines("input.txt");
        char[] seps = { ' ', ':' };
        foreach (string line in lines)
        {
            string[] nameArgs = line.Split(seps, StringSplitOptions.RemoveEmptyEntries);
            TreeOp monke;
            if (!monkeys.TryGetValue(nameArgs[0], out monke))
            {
                monke = new TreeOp(nameArgs[0]);
                monkeys[nameArgs[0]] = monke;
            }

            if (nameArgs.Length == 2)
            {
                monke.N = long.Parse(nameArgs[1]);
                monke.Op = 's';
            }
            else
            {
                TreeOp monkeL, monkeR;

                if (!monkeys.TryGetValue(nameArgs[1], out monkeL))
                {
                    monkeL = new TreeOp(nameArgs[1]);
                    monkeys[nameArgs[1]] = monkeL;
                }

                if (!monkeys.TryGetValue(nameArgs[3], out monkeR))
                {
                    monkeR = new TreeOp(nameArgs[3]);
                    monkeys[nameArgs[3]] = monkeR;
                }

                monke.Op = nameArgs[2][0];
                monke.Left = monkeL;
                monke.Right = monkeR;
                monkeL.Parent = monke;
                monkeR.Parent = monke;
            }

        }

        TreeOp root = monkeys["root"];
        TreeOp human = monkeys["humn"];

        List<TreeOp> pathToHuman = new List<TreeOp>();
        pathToHuman.Add(human);
        for (TreeOp curr = human.Parent; curr != root; curr = curr.Parent)
        {
            pathToHuman.Add(curr);
        }

        long target = (pathToHuman[pathToHuman.Count - 1] == root.Left ? root.Right.Shout() : root.Left.Shout());
        for (int i = pathToHuman.Count - 2; i >= 0; i--)
        {
            if (pathToHuman[i] == pathToHuman[i + 1].Left)
            {
                long rightValue = pathToHuman[i + 1].Right.Shout();
                switch (pathToHuman[i + 1].Op)
                {
                    case '+':
                        target -= rightValue;
                        break;
                    case '-':
                        target += rightValue;
                        break;
                    case '*':
                        target /= rightValue;
                        break;
                    case '/':
                        target *= rightValue;
                        break;
                }
            }
            else
            {
                long leftValue = pathToHuman[i + 1].Left.Shout();
                switch (pathToHuman[i + 1].Op)
                {
                    case '+':
                        target -= leftValue;
                        break;
                    case '-':
                        target = leftValue - target;
                        break;
                    case '*':
                        target /= leftValue;
                        break;
                    case '/':
                        target = leftValue/target;
                        break;
                }
            }
        }
        Console.WriteLine(target);
    }
}