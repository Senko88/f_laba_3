open System
open System.IO
/// Функция вычисления произведения цифр числа (с обработкой отрицательных чисел и нуля)
let digitProduct n =
    let absNum = abs n
    if absNum = 0 then 0
    else
        string absNum
        |> Seq.map (fun c -> int(c) - int('0'))  
        |> Seq.fold (fun acc digit -> acc * digit) 1  

/// Ввод последовательности чисел с клавиатуры
let inputNumbers () =
    printfn "Введите числа через пробел:"
    Console.ReadLine().Split(' ')
    |> Seq.choose (fun s -> 
        match Int32.TryParse(s) with
        | true, num -> Some num
        | _ -> 
            printfn $"Ошибка: '{s}' — не число. Пропускаем."
            None)

// Пример использования
let numbers = inputNumbers()
let products = numbers |> Seq.map digitProduct
printfn "Произведения цифр: %A" (products |> Seq.toList)








/// Проверка, что строка является двоичным числом
let isBinary (s: string) =
    s |> Seq.forall (fun c -> c = '0' || c = '1')

/// Преобразование двоичной строки в десятичное число
let binaryToDecimal binaryStr = Convert.ToInt32(binaryStr, 2)

/// Ввод последовательности двоичных чисел
let inputBinaryNumbers () =
    printfn "Введите двоичные числа через пробел:"
    Console.ReadLine().Split(' ')
    |> Seq.choose (fun s -> 
        let trimmed = s.Trim()
        if isBinary trimmed then Some trimmed
        else 
            printfn $"Ошибка: '{trimmed}' — не двоичное число. Пропускаем."
            None)

/// Сумма двоичных чисел (используем Seq.fold)
let sumBinaryNumbers binarySeq =
    binarySeq
    |> Seq.fold (fun acc binary -> acc + binaryToDecimal binary) 0

// Пример использования
let binaryNumbers = inputBinaryNumbers()
let totalSum = sumBinaryNumbers binaryNumbers
printfn "Сумма в десятичной системе: %d" totalSum










/// Безопасно проверяет существование файла в каталоге и подкаталогах
let isFileInDirectory (dir: string) (filename: string) =
    try
        Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories)
        |> Seq.exists (fun file -> 
            // Сравниваем имена файлов без учёта регистра
            String.Equals(Path.GetFileName(file), filename, StringComparison.OrdinalIgnoreCase))
    with
    | :? DirectoryNotFoundException ->
        printfn $"Ошибка: каталог '{dir}' не найден."
        false
    | :? UnauthorizedAccessException ->
        printfn $"Ошибка: нет доступа к каталогу '{dir}'."
        false

/// Запрашивает у пользователя путь и имя файла
let searchFile () =
    printfn "Введите путь к каталогу:"
    let dir = Console.ReadLine()
    
    printfn "Введите имя файла для поиска:"
    let filename = Console.ReadLine()
    
    match isFileInDirectory dir filename with
    | true -> printfn $"Файл '{filename}' найден в каталоге '{dir}'!"
    | false -> printfn $"Файл '{filename}' не найден."

// Запуск поиска
searchFile ()
