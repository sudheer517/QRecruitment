
module Mocks {
    import TestViewModel = Recruitment.ViewModels.TestViewModel;
    import QuestionViewModel = Recruitment.ViewModels.QuestionViewModel;
    import OptionViewModel = Recruitment.ViewModels.QuestionOptionViewModel;

    import TestDto = Quantium.Recruitment.ODataEntities.TestDto

    export class EditTestMock {

        public static getTestData(testId: number): TestViewModel {
            var test1 = new TestViewModel(1, "Bot team test", questions1);
            var questions1: QuestionViewModel[];

            questions1.push(new QuestionViewModel(
                1,
                "Who are you?",
                [new OptionViewModel("Elf", false), new OptionViewModel("Muggle", true), new OptionViewModel("Wizard", true), new OptionViewModel("Nazgul", false)],
                64))

            questions1.push(new QuestionViewModel(
                2,
                "Who killed the mad king?",
                [new OptionViewModel("Robert", false), new OptionViewModel("Cersei", true), new OptionViewModel("Jamie", false), new OptionViewModel("Ned", false)],
                76));

            questions1.push(new QuestionViewModel(
                3,
                "If 'i dont give a shit' means 'i dont care', then 'i give you shit' means 'i care about you'?",
                [new OptionViewModel("Yes", true), new OptionViewModel("No", true), new OptionViewModel("No idea mate!", false), new OptionViewModel("Fuck this Shit", true)],
                89));

            questions1.push(new QuestionViewModel(
                4,
                "If you had 3 oranges and 4 apples in one hand & 3 apples and 4 oranges in one hand, what would you have?",
                [new OptionViewModel("7 oranges", false), new OptionViewModel("7 apples ", false), new OptionViewModel("No idea mate!", false), new OptionViewModel("Very large hands", true)],
                43));

            questions1.push(new QuestionViewModel(
                5,
                "May the force be...",
                [new OptionViewModel("with you", true), new OptionViewModel("mass times acceleration", false), new OptionViewModel("mv/t", false), new OptionViewModel("None of the above", false)],
                21));

            var test2 = new TestViewModel(1, "Checkout test", questions2);
            var questions2: QuestionViewModel[];

            questions2.push(new QuestionViewModel(
                6,
                "Mental break time. This is very tough question. you dont deserve it. What is 1+1?",
                [new OptionViewModel("Not this one", false), new OptionViewModel("Still not this one", false), new OptionViewModel("2", true), new OptionViewModel("You've gone too far, go back to 3rd option", false)],
                15));

            questions2.push(new QuestionViewModel(
                7,
                "Who's intellectual property is this? e^ix = cos x + i sin x",
                [new OptionViewModel("Definitel not me", true), new OptionViewModel("S. Ramanujan", false), new OptionViewModel("Euler", false), new OptionViewModel("Jacobi", false)],
                16));

            questions2.push(new QuestionViewModel(
                8,
                "What happened when wheel was invented?",
                [new OptionViewModel("It rolled", false), new OptionViewModel("Do i need to answer it?", false), new OptionViewModel("it created a revolution", true), new OptionViewModel("No one bought it", false)],
                10));

            questions2.push(new QuestionViewModel(
                9,
                "Pick the odd one out",
                [new OptionViewModel("Egg1", true), new OptionViewModel("Egg2", true), new OptionViewModel("Egg3", true), new OptionViewModel("Egg4", true)],
                31));

            if (testId == test1.testId) return test1;
            if (testId == test2.testId) return test2;
            else return test1;
        }
    }
}