namespace workshop.domain

open System

module Say =
    let hello name =
        printfn "Hello %s" name

module Workshop =
    type Department = 
        | Engineering 
        | Geosciences
        | FineArts
        | NotFound
        member this.ToCode() =
            match this with
            | Engineering   -> 100
            | Geosciences   -> 200
            | FineArts      -> 300
            | NotFound      -> 0
        override this.ToString() =
            match this with
            | Engineering   -> "Engineering"
            | Geosciences   -> "Geosciences"
            | FineArts      -> "Fine Arts"
            | _             -> String.Empty

    let getDepartment code =
        match code with
        | 100   -> Engineering
        | 200   -> Geosciences
        | 300   -> FineArts
        | _     -> NotFound

    type CourseName = private CourseName of string
    module CourseName =
        let create (s:string) =
            match s.Trim() with
            | nm when nm.Length <= 100  -> CourseName nm
            | nm                        -> CourseName (nm.Substring(0, 100))
        let value (CourseName s) = s   

    type Course =
        {
            Number      : int
            Name        : CourseName
            Description : string
            Credits     : int
            Department  : Department
        }

    let course =
      {
          Number = 9999
          Name = CourseName.create "Underwater Basket Weaving"
          Description = "Traditional basket weaving done under water for best effect."
          Credits = 3
          Department = FineArts
      }
