///// <reference path="../typings/remoteservicesproxy.ts" />

//module Mocks {
//    import TestDto = Quantium.Recruitment.ODataEntities.TestDto;
//    import LabelDto = Quantium.Recruitment.ODataEntities.LabelDto;
//    import QuestionDto = Quantium.Recruitment.ODataEntities.QuestionDto;
//    import QuestionGroupDto = Quantium.Recruitment.ODataEntities.QuestionGroupDto;
//    import OptionDto = Quantium.Recruitment.ODataEntities.OptionDto;

//    export class EditTestMock {

//        //public static getTestData(testId: number): TestDto {
            
//        //    var questions = new Array<QuestionDto>();

//        //    questions.push(new QuestionDto(
//        //        1,
//        //        "Who are you?",
//        //        64,
//        //        new QuestionGroupDto(1, "Question Group1"),
//        //        [new OptionDto(51, 1, "Elf", false), new OptionDto(52, 1, "Muggle", true), new OptionDto(53, 1, "Wizard", true), new OptionDto(54, 1, "Nazgul", false)]))

//        //    questions.push(new QuestionDto(
//        //        2,
//        //        "Who killed the mad king?",
//        //        76,
//        //        new QuestionGroupDto(2, "Question Group2"),
//        //        [new OptionDto(55, 2, "Robert", false), new OptionDto(56, 2, "Cersei", true), new OptionDto(57, 2, "Jamie", false), new OptionDto(58, 2,"Ned", false)]
//        //        ));

//        //    questions.push(new QuestionDto(
//        //        3,
//        //        "If 'i dont give a shit' means 'i dont care', then 'i give you shit' means 'i care about you'?",
//        //        89,
//        //        new QuestionGroupDto(2, "Question Group2"),
//        //        [new OptionDto(59, 2, "Yes", true), new OptionDto(60, 2, "No", true), new OptionDto(61, 2, "No idea mate!", false), new OptionDto(62, 2, "Fuck this Shit", true)]
//        //        ));

//        //    questions.push(new QuestionDto(
//        //        4,
//        //        "If you had 3 oranges and 4 apples in one hand & 3 apples and 4 oranges in one hand, what would you have?",
//        //        43,
//        //        new QuestionGroupDto(3, "Question Group3"),
//        //        [new OptionDto(63, 3, "7 oranges", false), new OptionDto(64, 3, "7 apples ", false), new OptionDto(64, 3, "No idea mate!", false), new OptionDto(65, 3, "Very large hands", true)]
//        //        ));

//        //    questions.push(new QuestionDto(
//        //        5,
//        //        "May the force be...",
//        //        21,
//        //        new QuestionGroupDto(3, "Question Group3"),
//        //        [new OptionDto(66, 3, "with you", true), new OptionDto(67, 3, "mass times acceleration", false), new OptionDto(68, 3, "mv/t", false), new OptionDto(69, 3, "None of the above", false)]
//        //    ));
//        //    var test1 = new TestDto(1, "Test1", [new LabelDto(100, "Software", 1)], questions);

//        //    var questions2 = new Array<QuestionDto>();

//        //    questions2.push(new QuestionDto(
//        //        6,
//        //        "Mental break time. This is very tough question. you dont deserve it. What is 1+1?",
//        //        15,
//        //        new QuestionGroupDto(4, "Question Group4"),
//        //        [new OptionDto(70, 4, "Not this one", false), new OptionDto(71, 4, "Still not this one", false), new OptionDto(72, 4, "2", true), new OptionDto(73, 4, "You've gone too far, go back to 3rd option", false)]
//        //        ));

//        //    questions2.push(new QuestionDto(
//        //        7,
//        //        "Who's intellectual property is this? e^ix = cos x + i sin x",
//        //        16,
//        //        new QuestionGroupDto(4, "Question Group4"),
//        //        [new OptionDto(74, 4, "Definitel not me", true), new OptionDto(75, 4 ,"S. Ramanujan", false), new OptionDto(76, 4, "Euler", false), new OptionDto(77, 4, "Jacobi", false)]
//        //        ));

//        //    questions2.push(new QuestionDto(
//        //        8,
//        //        "What happened when wheel was invented?",
//        //        10,
//        //        new QuestionGroupDto(5, "Question Group5"),
//        //        [new OptionDto(78, 5, "It rolled", false), new OptionDto(79, 5,"Do i need to answer it?", false), new OptionDto(80, 5, "it created a revolution", true), new OptionDto(81, 5, "No one bought it", false)]
//        //        ));

//        //    questions2.push(new QuestionDto(
//        //        9,
//        //        "Pick the odd one out",
//        //        31,
//        //        new QuestionGroupDto(5, "Question Group5"),
//        //        [new OptionDto(82, 5, "Egg1", true), new OptionDto(83, 5, "Egg2", true), new OptionDto(84, 5, "Egg3", true), new OptionDto(85, 5, "Egg4", true)]
//        //        ));

//        //    var test2 = new TestDto(2, "Checkout test", [new LabelDto(101, "Analytics", 2)], questions2);

//        //    if (testId == test1.Id) return test1;
//        //    if (testId == test2.Id) return test2;
//        //    else return test1;
//        //}
//    }
//}