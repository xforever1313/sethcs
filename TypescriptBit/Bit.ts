//
//          Copyright Seth Hendrick 2019.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

interface ITestFixture {

    // ---------------- Properties ----------------

    FixtureName: string;

    // ---------------- Functions  ----------------

    DoFixtureSetup(): void;

    DoFixtureTeardown(): void;

    DoTestSetup(): void;

    DoTestTeardown(): void;

    GetAllTests(): Array<Test>;
}

class TestFailureException {

    constructor(message: string) {
        this.Message = message;
    }

    Message: string;

    public toString(): string {
        return this.Message;
    }
}

class Assert {

    public static IsTrue(assert: boolean): void {
        if (assert === false) {
            throw new TestFailureException("Passed in assertion was false, expected true");
        }
    }

    public static IsFalse(assert: boolean): void {
        if (assert === true) {
            throw new TestFailureException("Passed in assertion was true, expected false");
        }
    }

    public static AreEqual(obj1: any, obj2: any) {
        if (obj1 !== obj2) {
            throw new TestFailureException(
                "Expected: " + obj1 + ", Actual: " + obj2
            );
        }
    }
}

class Test {

    // ---------------- Fields ----------------

    private readonly action: Function;

    // ---------------- Constructor ----------------

    constructor(name: string, action: Function) {
        this.Name = name;
        this.action = action;
    }

    // ---------------- Properties ---------------

    public Name: string;

    // ---------------- Functions ----------------

    DoTest(): void {
        this.action();
    }
}

class TestRunner {

    // ---------------- Fields ----------------

    private static instance: TestRunner;

    private readonly testFixtures: Array<ITestFixture>;

    // ---------------- Constructor ----------------

    private constructor() {
        this.testFixtures = new Array<ITestFixture>();
    }

    public static Instance(): TestRunner{
        if(this.instance === undefined){
            this.instance = new TestRunner();
        }

        return this.instance;
    }

    public AddTestFixture(fixture: ITestFixture): void{
        this.testFixtures.push(fixture);
    }

    public AddTestFixtures(fixtures: Array<ITestFixture>): void {
        for(let fix of fixtures){
            this.AddTestFixture(fix);
        }
    }

    public Execute(): TestResults {
        let results: TestResults = new TestResults();

        for(let fix of this.testFixtures){
            try{
                fix.DoFixtureSetup();
                
                for(let test of fix.GetAllTests()){
                    try{
                        fix.DoTestSetup();
                        test.DoTest();
                        results.AddPass(test.Name);
                    }
                    catch(e){
                        results.AddFail(test.Name + ": " + e);
                    }
                    finally{
                        fix.DoTestTeardown();
                    }
                }
            }
            catch(e){
                results.AddFail(fix.FixtureName + ": " + e);
            }
            finally{
                try{
                    fix.DoFixtureTeardown();
                }
                catch{
                }
            }
        }

        return results;
    }
}

class TestResults{

    // ---------------- Constructor ----------------

    public constructor() {
        this.Passes = new Array<string>();
        this.Fails = new Array<string>();
    }

    // ---------------- Properties ----------------

    public Passes: Array<string>;
    public Fails: Array<string>;

    // ---------------- Functions ----------------

    public AddPass(message: string): void {
        this.Passes.push(message);
    }

    public AddFail(message: string): void {
        this.Fails.push(message);
    }
}

class TestResultDisplayer {

    // ---------------- Fields ----------------

    private readonly messageDiv: HTMLDivElement;
    private readonly overallDiv: HTMLDivElement;

    // ---------------- Constructor ----------------

    constructor(messageDiv: HTMLDivElement, overallDiv: HTMLDivElement) {
        this.messageDiv = messageDiv;
        this.overallDiv = overallDiv;
    }

    // ---------------- Functions ----------------

    DisplayResults(results: TestResults) {
        let list: HTMLUListElement = document.createElement("ul");
        for (let pass of results.Passes) {
            let listElement: HTMLLIElement = document.createElement("li");
            listElement.innerText = "Pass: " + pass;
            list.appendChild(listElement);
        }

        for (let fail of results.Fails) {
            let listElement: HTMLLIElement = document.createElement("li");
            listElement.innerText = "Fail: " + fail;
            list.appendChild(listElement);
        }

        this.messageDiv.appendChild(list);
        this.overallDiv.innerText = "Passes: " + results.Passes.length + ", Failures: " + results.Fails.length;
    }
}
