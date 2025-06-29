open System
open System.IO
// Функция для вычисления произведения цифр
let digitProduct n =
    if n = 0 then 0
    else
        string (abs n)
        |> Seq.map (fun c -> int c - int '0')
        |> Seq.reduce (*)

// Ввод чисел
let inputNumbersSeq () =
    printfn "Введите числа через пробел:"
    Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries)
    |> Seq.choose (fun s ->
        match Int32.TryParse(s) with
        | true, num -> Some num
        | _ -> 
            printfn $"Ошибка: '{s}' не является числом. Пропускаем."
            None)

// Отложенные вычисления
let lazyProducts numbers =
    numbers
    |> Seq.map digitProduct
    |> Seq.cache

// Главная программа
let numbersSeq = inputNumbersSeq()
let productsSeq = lazyProducts numbersSeq

// Вывод в одну строку через запятую
printfn "Произведения цифр: %s" (String.Join(", ", productsSeq))





// Проверка, что строка — двоичное число
let isBinary (s: string) = s |> Seq.forall (fun c -> c = '0' || c = '1')

// Ввод двоичных чисел (возвращает seq<string>)
let inputBinarySeq () =
    printfn "Введите двоичные числа через пробел:"
    let input = Console.ReadLine()
    input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
    |> Seq.filter isBinary

// Преобразование двоичной строки в число (через Seq.fold)
let binaryToInt bin =
    bin
    |> Seq.fold (fun acc c -> acc * 2 + (if c = '1' then 1 else 0)) 0

// Отложенная сумма (через Seq.map + Seq.fold)
let lazySumBinaries binaries =
    binaries
    |> Seq.map binaryToInt
    |> Seq.cache
    |> Seq.fold (+) 0

// Пример
let binarySeq = inputBinarySeq()
let totalSum = lazySumBinaries binarySeq
printfn "Сумма двоичных чисел: %d" totalSum




// Функция для поиска файла в каталоге и подкаталогах
let findFileInDirectory (dirPath: string) (targetFile: string) =
    try
        // Проверяем существование каталога сразу
        if not (Directory.Exists dirPath) then
            false
        else
            // Ищем файл рекурсивно
            Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories)
            |> Seq.exists (fun file -> 
                Path.GetFileName(file).Equals(targetFile, StringComparison.OrdinalIgnoreCase))
    with
    | :? UnauthorizedAccessException -> false

// Основная программа
printfn "Введите путь к каталогу:"
let dirPath = Console.ReadLine().Trim()

printfn "Введите имя файла для поиска:"
let fileName = Console.ReadLine().Trim()

try
    let isFound = findFileInDirectory dirPath fileName
    printfn "Файл '%s' %s" fileName (if isFound then "найден" else "не найден")
with
| :? DirectoryNotFoundException -> 
    printfn "Ошибка: каталог '%s' не существует!" dirPath
| ex ->
    printfn "Произошла ошибка: %s" ex.Message
