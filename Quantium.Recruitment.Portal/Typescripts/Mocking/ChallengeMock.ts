///// <reference path="../viewmodels/challengeoptionviewmodel.ts" />
///// <reference path="../viewmodels/challengeviewmodel.ts" />

//module Mocks {
//    import OptionViewModel = Recruitment.ViewModels.ChallengeOptionViewModel;

//    export class ChallengeMock {

//        public static getNextQuestion(): Recruitment.ViewModels.ChallengeViewModel {

//            var challenges: Recruitment.ViewModels.ChallengeViewModel[] = [];

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                1,
//                "Who are you?",
//                [new OptionViewModel(25, "Elf"), new OptionViewModel(26, "Muggle"), new OptionViewModel(27, "Wizard"), new OptionViewModel(28, "Nazgul")],
//                64));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                2,
//                "Who killed the mad king?",
//                [new OptionViewModel(29, "Robert"), new OptionViewModel(30, "Cersei"), new OptionViewModel(31, "Jamie"), new OptionViewModel(32, "Ned")],
//                76));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                3,
//                "If 'i dont give a shit' means 'i dont care', then 'i give you shit' means 'i care about you'?",
//                [new OptionViewModel(33, "Yes"), new OptionViewModel(34, "No"), new OptionViewModel(35, "No idea mate!"), new OptionViewModel(36, "Fuck this Shit")],
//                89));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                4,
//                "If you had 3 oranges and 4 apples in one hand & 3 apples and 4 oranges in one hand, what would you have?",
//                [new OptionViewModel(37, "7 oranges"), new OptionViewModel(38, "7 apples "), new OptionViewModel(39, "No idea mate!"), new OptionViewModel(40, "Very large hands")],
//                43));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                5,
//                "May the force be...",
//                [new OptionViewModel(41, "with you"), new OptionViewModel(42, "mass times acceleration"), new OptionViewModel(43, "mv/t"), new OptionViewModel(44, "None of the above")],
//                21));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                6,
//                "Mental break time. This is very tough question. you dont deserve it. What is 1+1?",
//                [new OptionViewModel(45, "Not this one"), new OptionViewModel(46, "Still not this one"), new OptionViewModel(47, "2"), new OptionViewModel(48, "You've gone too far, go back to 3rd option")],
//                15));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                7,
//                "Who's intellectual property is this? e^ix = cos x + i sin x",
//                [new OptionViewModel(49, "Definitel not me"), new OptionViewModel(50, "S. Ramanujan"), new OptionViewModel(51, "Euler"), new OptionViewModel(52, "Jacobi")],
//                16));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                8,
//                "What happened when wheel was invented?",
//                [new OptionViewModel(53, "It rolled"), new OptionViewModel(54, "Do i need to answer it?"), new OptionViewModel(55, "it created a revolution"), new OptionViewModel(56, "No one bought it")],
//                10));

//            challenges.push(new Recruitment.ViewModels.ChallengeViewModel(
//                9,
//                "Pick the odd one out",
//                [new OptionViewModel(57, "Egg1"), new OptionViewModel(58, "Egg2"), new OptionViewModel(59, "Egg3"), new OptionViewModel(60, "Egg4")],
//                31));

//            return challenges[Math.floor(Math.random() * challenges.length)];
//        }

//    }
//}