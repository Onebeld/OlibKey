# How to contribute
Not all programs are perfect. Each has its flaws and bugs. OlibKey is no exception.

A proposal for a new function is always welcome, but it must be clearly formulated what it will be. Here are the guidelines for correctly proposing a new feature in the program:
- Roughly name your function;
- It is advisable to describe in detail why she is;
- (Optional) You will be able to make the window / page layout of this new function.

The new functions are, of course, good, but there is no way without bugs. If the application is large enough, bugs are harder to spot. You users will come to the rescue. Here is the recommended list for correct error reporting:
- Make sure to write the program version, OS and architecture of the program and OS (e.g. 3.1.1.0 x86, Windows 10 x64);
- Describe the problem;
- If you are familiar with the code, you can even suggest changes to fix this error.

## Translations
Not all users are proficient in the secondary language. You can offer a translation into another language. What should be done?
- Take the original translation from this folder: `Assets\Local`;
- Translate in the following way: `<v:String x:Key="(Don't change here)">(Your translation)</v:String>`;
- Name your translation new language. It should look like this, for example with English: `lang.en-US.axaml`;
- Create a request to add a translation. Name the language.
