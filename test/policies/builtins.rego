package builtins

firstValue := custom.builtin0()

secondValue := custom.builtin1(firstValue)

thirdValue := custom.builtin2(firstValue, secondValue)

fourthValue := custom.builtin3(firstValue, secondValue, thirdValue)

fifthValue := custom.builtin4(firstValue, secondValue, thirdValue, fourthValue)

sdkBuiltinValue := type_name(fifthValue)
