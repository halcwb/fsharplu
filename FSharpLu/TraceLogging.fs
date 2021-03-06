﻿/// Copyright (c) Microsoft Corporation.
/// Functors and types used to create strongly-typed Printf-like wrappers
/// for Trace logging and System.Diagnostics
namespace Microsoft.FSharpLu.Logging

    /// Given an implementation of the following `TraceWriter` trait (or "static interface") over a static type ^T,
    /// construct a _strongly-typed formatter tracer_ StronglyTypedTracer< ^T>.
    ///
    /// The `TraceWriter` trait is defined as:
    ///      ^T when ^T : (static member writeLine : string -> unit)
    ///          and ^T : (static member info : string -> unit)
    ///          and ^T : (static member warning : string -> unit)
    ///          and ^T : (static member error : string -> unit)
    ///          and ^T : (static member critical : string -> unit)
    ///          and ^T : (static member verbose : string -> unit)
    ///          and ^T : (static member writeLine : string -> unit)
    ///          and ^T : (static member flush : unit -> unit)
    ///          and ^T : (static member indent : unit -> unit)
    ///          and ^T : (static member unindent : unit -> unit)
    /// Unfortunately, F# does not support definition of aliases for sets of static constraints so
    /// we have to repeat the trait definition everytime we refer to this interface. Two related uservoices
    /// suggestions were declined by the F# team:
    ///   https://fslang.uservoice.com/forums/245727-f-language/suggestions/8509687-add-constraints-as-a-language-construct
    ///   https://fslang.uservoice.com/forums/245727-f-language/suggestions/8393964-interfaces-as-simple-reusable-and-named-sets-of-m
    /// But maybe one day we will be able to use type-classes instead:
    ///   https://fslang.uservoice.com/forums/245727-f-language/suggestions/5762135-support-for-type-classes-or-implicits

    /// Functor used to create a strongly-typed event tracer
    /// from a TraceWriter trait
    type StronglyTypedTracer< ^T when
                           // ^T implements the `TraceWriter` trait
                            ^T : (static member writeLine : string -> unit)
                        and ^T : (static member info : string -> unit)
                        and ^T : (static member warning : string -> unit)
                        and ^T : (static member error : string -> unit)
                        and ^T : (static member critical : string -> unit)
                        and ^T : (static member verbose : string -> unit)
                        and ^T : (static member writeLine : string -> unit)
                        and ^T : (static member flush : unit -> unit)
                        and ^T : (static member indent : unit -> unit)
                        and ^T : (static member unindent : unit -> unit)
                        //// 
                        > =
        static member inline info format = Printf.kprintf (fun m -> (^T:(static member info : string -> unit) m)) format
        static member inline warning format = Printf.kprintf (fun m -> (^T:(static member warning : string -> unit) m)) format
        static member inline error format = Printf.kprintf (fun m -> (^T:(static member error : string -> unit) m)) format
        static member inline critical format = Printf.kprintf (fun m -> (^T:(static member critical : string -> unit) m)) format
        static member inline failwith format = Printf.kprintf (fun m -> (^T:(static member critical : string -> unit) m); Operators.failwith m) format
        static member inline verbose format = Printf.kprintf (fun m -> (^T:(static member verbose : string -> unit) m)) format
        static member inline writeLine format  = Printf.kprintf (fun m -> (^T:(static member writeLine : string -> unit) m)) format
        static member inline flush () = (^T:(static member flush : unit -> unit) ())
        static member inline indent () =  (^T:(static member indent : unit -> unit) ())
        static member inline unindent () = (^T:(static member unindent : unit -> unit) ())

    /// Combine two TraceWriter trait implementations into one
    type Combine< ^T1, ^T2 when
                            ^T1 : (static member writeLine : string -> unit)
                        and ^T1 : (static member info : string -> unit)
                        and ^T1 : (static member warning : string -> unit)
                        and ^T1 : (static member error : string -> unit)
                        and ^T1 : (static member critical : string -> unit)
                        and ^T1 : (static member verbose : string -> unit)
                        and ^T1 : (static member writeLine : string -> unit)
                        and ^T1 : (static member flush : unit -> unit)
                        and ^T1 : (static member indent : unit -> unit)
                        and ^T1 : (static member unindent : unit -> unit)

                        and ^T2 : (static member writeLine : string -> unit)
                        and ^T2 : (static member info : string -> unit)
                        and ^T2 : (static member warning : string -> unit)
                        and ^T2 : (static member error : string -> unit)
                        and ^T2 : (static member critical : string -> unit)
                        and ^T2 : (static member verbose : string -> unit)
                        and ^T2 : (static member writeLine : string -> unit)
                        and ^T2 : (static member flush : unit -> unit)
                        and ^T2 : (static member indent : unit -> unit)
                        and ^T2 : (static member unindent : unit -> unit)> =
        static member inline info m =
            (^T1:(static member info : string -> unit) m)
            (^T2:(static member info : string -> unit) m)
        static member inline warning m =
            (^T1:(static member warning : string -> unit) m)
            (^T2:(static member warning : string -> unit) m)
        static member inline error m =
            (^T1:(static member error : string -> unit) m)
            (^T2:(static member error : string -> unit) m)
        static member inline critical m =
            (^T1:(static member critical : string -> unit) m)
            (^T2:(static member critical : string -> unit) m)
        static member inline verbose m =
            (^T1:(static member verbose : string -> unit) m)
            (^T2:(static member verbose : string -> unit) m)
        static member inline writeLine m =
            (^T1:(static member writeLine : string -> unit) m)
            (^T2:(static member writeLine : string -> unit) m)
        static member inline flush () =
            (^T1:(static member flush : unit -> unit) ())
            (^T2:(static member flush : unit -> unit) ())
        static member inline indent () =
            (^T1:(static member indent : unit -> unit) ())
            (^T2:(static member indent : unit -> unit) ())
        static member inline unindent () =
            (^T1:(static member unindent : unit -> unit) ())
            (^T2:(static member unindent : unit -> unit) ())

    module EnvironmentInfo =
        /// Trace system environment information using the specified strongly-typed tracer
        let inline trace< ^T when 
                        // ^T implements the `TraceWriter` trait
                        ^T : (static member writeLine : string -> unit)
                        and ^T : (static member info : string -> unit)
                        and ^T : (static member warning : string -> unit)
                        and ^T : (static member error : string -> unit)
                        and ^T : (static member critical : string -> unit)
                        and ^T : (static member verbose : string -> unit)
                        and ^T : (static member writeLine : string -> unit)
                        and ^T : (static member flush : unit -> unit)
                        and ^T : (static member indent : unit -> unit)
                        and ^T : (static member unindent : unit -> unit)
                        ////
                    > () =
            StronglyTypedTracer< ^T>.indent()
            StronglyTypedTracer< ^T>.writeLine "Operating system: %O" System.Environment.OSVersion
            StronglyTypedTracer< ^T>.writeLine "Computer name: %s" System.Environment.MachineName
            StronglyTypedTracer< ^T>.writeLine "User name: %s" System.Environment.UserName
            StronglyTypedTracer< ^T>.writeLine "CLR runtime version: %O" System.Environment.Version
            StronglyTypedTracer< ^T>.writeLine "Command line: %s" System.Environment.CommandLine
            StronglyTypedTracer< ^T>.unindent()

/// Helper functions for System.Diagnostics
namespace System.Diagnostics
    open System

    /// Trace logging configuration passed to registerFileAndConsoleTracerWithConfiguration
    type LoggingConfiguration<'A> =
        {
            /// Title printed on the console right after the registration completes
            title : string
            /// Name of the component using the tracing functions
            componentName : string
            /// Path to the directory where to write the log file
            directory : string option
            /// Trace options passed to the auxiliary listener
            traceOptions : System.Diagnostics.TraceOptions
            /// Tracing level
            auxiliaryTraceLevel : System.Diagnostics.TraceLevel
            /// Parameters used by the auxiliary trace listener
            auxiliaryConfiguration : 'A
        }

    /// A strongly-typed tracer implemented with System.Diagnostics event tracing
    type DiagnosticsTracer =
        static member inline info m = Trace.TraceInformation m
        static member inline warning m = Trace.TraceWarning m
        static member inline error m = Trace.TraceError m
        static member inline verbose m = Debug.Write m
        static member inline writeLine m = Trace.WriteLine m
        static member inline critical m = Trace.TraceError m
        static member inline flush () = Trace.Flush()
        static member inline indent () =  Trace.Indent()
        static member inline unindent () = Trace.Unindent()

    /// Interface for anything that can be flushed (e.g., stream)
    type IFlushable =
        abstract Flush : unit -> unit

    /// Self-cleanup interface for objects returned by logger registration functions
    type IFileLogger =
        inherit IDisposable
        inherit IFlushable
        abstract LogFilePath : string

    /// FSharpLu helpers to register listers with System.Diagnostics
    module Listener =
        [<Literal>]
        let TraceLogFilePathKey = "LogFilePath"

        /// Register a trace listener that redirects all tracing functions from System.Diagnostics
        /// to an external log file on disk.
        /// Returns an IDisposable that, when disposed, unregisters the listener.
        let registerFileTracer (componentName:System.String) directory =
            let cleansedName =
                System.IO.Path.GetInvalidFileNameChars()
                |> Seq.fold (fun (t:System.String) c -> t.Replace(string c, "")) (componentName.Replace(" ", ""))
            let directory = defaultArg directory <| System.IO.Directory.GetCurrentDirectory()
            let logFileName = sprintf "%s-%s.log" cleansedName (System.DateTime.Now.ToString("yyyyMMdd-hhmmss"))
            let logFilePath = System.IO.Path.Combine (directory, logFileName)

            let logFileStream = new System.IO.FileStream(logFilePath, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite)
            let fileTracer = new System.Diagnostics.TextWriterTraceListener(logFileStream, Name = componentName, TraceOutputOptions = TraceOptions.DateTime)
            fileTracer.Attributes.Add(TraceLogFilePathKey, logFilePath)
            System.Diagnostics.Trace.Listeners.Add(fileTracer) |> ignore
            {
              new IFileLogger with
                member __.LogFilePath
                    with get () = logFilePath

              interface IFlushable with
                member __.Flush() =
                    fileTracer.Flush()
                    logFileStream.Flush()

              interface IDisposable with
                member __.Dispose() =
                    fileTracer.Flush()
                    logFileStream.Flush()
                    System.Diagnostics.Trace.Listeners.Remove(fileTracer)
                    fileTracer.Dispose()
                    logFileStream.Dispose()
            }

        /// Registers both a file tracer as well as an auxiliary tracer constructed from the specified parameters.
        let inline registerFileAndAuxiliaryTracerWithConfiguration< ^T, ^C when ^T :> TextWriterTraceListener > (loggerConstructor:LoggingConfiguration< ^C> -> ^T) (parameters:LoggingConfiguration< ^C>) =
            let fileLogger =
                // Reuse existing auxiliary listener if one is already registered
                let existingFileListener =
                    System.Diagnostics.Trace.Listeners
                    |> Seq.cast<TraceListener>
                    |> Seq.tryFind (fun l -> l.GetType() = typeof<TextWriterTraceListener> && l.Name = parameters.componentName)

                match existingFileListener with
                | None ->
                    registerFileTracer parameters.componentName parameters.directory
                | Some existingFileLogger ->
                    {
                      new IFileLogger with
                        member __.LogFilePath
                            with get () = existingFileLogger.Attributes.[TraceLogFilePathKey]

                      interface IFlushable with
                        member __.Flush() = ()

                      interface IDisposable with
                        member __.Dispose() = ()
                    }

            // Determine if an auxliary listener is already registered
            let existingAuxiliaryListener =
                System.Diagnostics.Trace.Listeners
                |> Seq.cast<TraceListener>
                |> Seq.tryFind (fun l -> l.GetType() = typeof< ^T>)

            match existingAuxiliaryListener with
            | Some auxiliaryTracer ->
                auxiliaryTracer.Attributes.["traceLevel"] <- parameters.auxiliaryTraceLevel.ToString()
                fileLogger
            | None ->
                // Create and register the auxiliary listener
                let auxiliaryTracer = loggerConstructor parameters

                auxiliaryTracer.Attributes.["traceLevel"] <- parameters.auxiliaryTraceLevel.ToString()
                auxiliaryTracer.WriteLine(sprintf "%O - [%s] - Starting output to trace listener." System.DateTime.Now auxiliaryTracer.Name)
                System.Diagnostics.Trace.Listeners.Add(auxiliaryTracer) |> ignore
                Trace.WriteLine parameters.title
                Microsoft.FSharpLu.Logging.EnvironmentInfo.trace<DiagnosticsTracer> ()
                // Return a disposable that will dispose both the auxiliary and file tracers
                {
                  new IFileLogger with
                    member __.LogFilePath
                        with get () = fileLogger.LogFilePath

                  interface IFlushable with
                    member __.Flush() =
                        auxiliaryTracer.Flush()
                        fileLogger.Flush()

                  interface IDisposable with
                    member __.Dispose() =
                        fileLogger.Dispose()
                        System.Diagnostics.Trace.Listeners.Remove(auxiliaryTracer)
                        auxiliaryTracer.Dispose()
                }



namespace Microsoft.FSharpLu.Logging
    /// Strongly-typed wrappers for System.Diagnostics event tracing
    type Trace = Microsoft.FSharpLu.Logging.StronglyTypedTracer<System.Diagnostics.DiagnosticsTracer>

    /// Strongly-typed event tracings with additional custom tags
    module TraceTags =
        /// Type used for tags associated with a logging message 
        type Tags = (string * string) list

        let propertiesToString (properties:seq<string * string>) =
            properties
            |> Seq.map (fun (n,v) -> sprintf "%s: %s" n v)
            |> Microsoft.FSharpLu.Text.join ", "

        let inline info name properties = Trace.info "%s: %s" name (propertiesToString properties)
        let inline warning name properties = Trace.warning "%s: %s" name (propertiesToString properties)
        let inline error name properties = Trace.error "%s: %s" name (propertiesToString properties)
        let inline verbose name properties = Trace.verbose "%s: %s" name (propertiesToString properties)
        let inline critical name properties = Trace.critical "%s: %s" name (propertiesToString properties)
        let inline failwith name properties = Trace.failwith "%s: %s" name (propertiesToString properties)
        let inline event name properties = Trace.writeLine "Event: %s: %s" name (propertiesToString properties)
        let inline trackException (exn:System.Exception) properties = Trace.critical "Exception: %O: %s" exn (propertiesToString properties)

        /// Combine two implementations of the `TraceTags` trait
        type Combine< ^T1, ^T2 when
                            // ^T1 implements the `TraceTags` trait
                                ^T1 : (static member writeLine : string -> Tags -> unit)
                            and ^T1 : (static member info : string -> Tags -> unit)
                            and ^T1 : (static member warning : string -> Tags -> unit)
                            and ^T1 : (static member error : string -> Tags -> unit)
                            and ^T1 : (static member critical : string -> Tags -> unit)
                            and ^T1 : (static member verbose : string -> Tags -> unit)
                            and ^T1 : (static member event : string -> Tags -> unit)
                            and ^T1 : (static member flush : unit -> unit)
                            and ^T1 : (static member indent : unit -> unit)
                            and ^T1 : (static member unindent : unit -> unit)
                            and ^T1 : (static member trackException : System.Exception -> Tags -> unit)
                    
                            // ^T2 implements the `TraceTags` trait
                            and ^T2 : (static member writeLine : string -> Tags -> unit)
                            and ^T2 : (static member info : string -> Tags -> unit)
                            and ^T2 : (static member warning : string -> Tags -> unit)
                            and ^T2 : (static member error : string -> Tags -> unit)
                            and ^T2 : (static member critical : string -> Tags -> unit)
                            and ^T2 : (static member verbose : string -> Tags -> unit)
                            and ^T2 : (static member event : string -> Tags -> unit)
                            and ^T2 : (static member flush : unit -> unit)
                            and ^T2 : (static member indent : unit -> unit)
                            and ^T2 : (static member unindent : unit -> unit)
                            and ^T2 : (static member trackException : System.Exception -> Tags -> unit)
                            > =
            static member inline writeLine m t =
                (^T1:(static member writeLine : string -> Tags -> unit) m, t)
                (^T2:(static member writeLine : string -> Tags -> unit) m, t)
            static member inline info m t =
                (^T1:(static member info : string -> Tags -> unit) m, t)
                (^T2:(static member info : string -> Tags -> unit) m, t)
            static member inline warning m t =
                (^T1:(static member warning : string -> Tags -> unit) m, t)
                (^T2:(static member warning : string -> Tags -> unit) m, t)
            static member inline error m t =
                (^T1:(static member error : string -> Tags -> unit) m, t)
                (^T2:(static member error : string -> Tags -> unit) m, t)
            static member inline critical m t =
                (^T1:(static member critical : string -> Tags -> unit) m, t)
                (^T2:(static member critical : string -> Tags -> unit) m, t)
            static member inline failwith m t =
                (^T1:(static member error : string -> Tags -> unit) m, t)
                (^T2:(static member error : string -> Tags -> unit) m, t)
                Operators.failwith m
            static member inline verbose m t =
                (^T1:(static member verbose : string -> Tags -> unit) m, t)
                (^T2:(static member verbose : string -> Tags -> unit) m, t)
            static member inline event m t =
                (^T1:(static member event : string -> Tags -> unit) m, t)
                (^T2:(static member event : string -> Tags -> unit) m, t)
            static member inline trackException (exn:System.Exception) t =
                (^T1:(static member trackException : System.Exception -> Tags -> unit) exn, t)
                (^T2:(static member trackException : System.Exception -> Tags -> unit) exn, t)
            static member inline flush () =
                (^T1:(static member flush : unit -> unit) ())
                (^T2:(static member flush : unit -> unit) ())
            static member inline indent () =
                (^T1:(static member indent : unit -> unit) ())
                (^T2:(static member indent : unit -> unit) ())
            static member inline unindent () =
                (^T1:(static member unindent : unit -> unit) ())
                (^T2:(static member unindent : unit -> unit) ())

namespace System.Diagnostics
    open Microsoft.FSharpLu.Logging

    /// A tracer outputing messages with tags to System.Diagnostics
    type TagsTracer =
        static member inline writeLine (m, t) = TraceTags.info m t
        static member inline info (m, t) = TraceTags.info m t
        static member inline warning (m, t) = TraceTags.warning m t
        static member inline error (m, t) = TraceTags.error m t
        static member inline critical (m, t) = TraceTags.critical m t
        static member inline failwith (m, t) = TraceTags.failwith m t
        static member inline verbose (m, t) = TraceTags.verbose m t
        static member inline trackException (exn, t) = TraceTags.trackException exn t
        static member inline event (name, t) = TraceTags.event name t
        static member inline flush () = Trace.flush()
        static member inline indent () = Trace.indent()
        static member inline unindent () = Trace.unindent()


namespace Microsoft.FSharpLu.Logging

/// Trace logging exposed as interface instead of via static modules
module Interfaces =

    /// TagTracer defined has an interface, used to pass tracing functions as parameters to other functions
    /// (as opposed to a static SRTP-based module as defined above)
    type ITagsTracer =
        abstract member writeLine : string -> TraceTags.Tags -> unit
        abstract member info : string -> TraceTags.Tags -> unit
        abstract member warning : string -> TraceTags.Tags -> unit
        abstract member error : string -> TraceTags.Tags -> unit
        abstract member critical : string -> TraceTags.Tags -> unit
        abstract member verbose : string -> TraceTags.Tags -> unit
        abstract member event : string -> TraceTags.Tags -> unit
        abstract member flush : unit -> unit
        abstract member indent : unit -> unit
        abstract member unindent : unit -> unit
        abstract member trackException : System.Exception -> TraceTags.Tags -> unit
        abstract member failwith : string -> TraceTags.Tags -> 'a

    /// Tracer defined has an interface, used to pass tracing functions as parameters to other functions
    /// (as opposed to a static SRTP-based module as defined above)
    type ITracer =
        abstract member writeLine : Printf.StringFormat<'a, unit> -> 'a
        abstract member info : Printf.StringFormat<'a, unit> -> 'a
        abstract member warning : Printf.StringFormat<'a, unit> ->  'a
        abstract member error : Printf.StringFormat<'a, unit> -> 'a
        abstract member critical : Printf.StringFormat<'a, unit> -> 'a
        abstract member verbose : Printf.StringFormat<'a, unit> -> 'a
        abstract member event : Printf.StringFormat<'a, unit> -> 'a
        abstract member flush : unit -> unit
        abstract member indent : unit -> unit
        abstract member unindent : unit -> unit
        abstract member trackException : System.Exception -> unit
        abstract member failwith<'a> : Printf.StringFormat<'a, unit> -> 'a

    // Return an implementation of interface ITracer from a type implementing the static `TraceWriter` trait
    let inline fromTraceWriter< ^T when 
                    // ^T implements the `TraceWriter` trait
                            ^T : (static member writeLine : string -> unit)
                        and ^T : (static member info : string -> unit)
                        and ^T : (static member warning : string -> unit)
                        and ^T : (static member error : string -> unit)
                        and ^T : (static member critical : string -> unit)
                        and ^T : (static member verbose : string -> unit)
                        and ^T : (static member event : string -> unit)
                        and ^T : (static member writeLine : string -> unit)
                        and ^T : (static member flush : unit -> unit)
                        and ^T : (static member indent : unit -> unit)
                        and ^T : (static member unindent : unit -> unit)
                        and ^T : (static member trackException : System.Exception -> unit)> =
        {
            new ITracer with
                member __.info<'a> format = StronglyTypedTracer< ^T>.info<'a> format
                member __.warning format = StronglyTypedTracer< ^T>.warning format
                member __.error format = StronglyTypedTracer< ^T>.error format
                member __.critical format = StronglyTypedTracer< ^T>.critical format
                member __.failwith format = StronglyTypedTracer< ^T>.failwith format
                member __.verbose format = StronglyTypedTracer< ^T>.verbose format
                member __.event format = StronglyTypedTracer< ^T>.writeLine format
                member __.writeLine format  = StronglyTypedTracer< ^T>.writeLine format
                member __.flush () = StronglyTypedTracer< ^T>.flush()
                member __.indent () =  StronglyTypedTracer< ^T>.indent()
                member __.unindent () = StronglyTypedTracer< ^T>.unindent()
                member __.trackException e = StronglyTypedTracer< ^T>.error "Exception: %O" e
        }

    // Return an implementation of interface ITagsTracer from a type implementing the static `TraceTags` trait
    let inline fromTraceTag< ^T when 
                    // ^T implements the `TraceTags` trait
                        ^T : (static member writeLine : string -> TraceTags.Tags -> unit)
                    and ^T : (static member info : string -> TraceTags.Tags -> unit)
                    and ^T : (static member warning : string -> TraceTags.Tags -> unit)
                    and ^T : (static member error : string -> TraceTags.Tags -> unit)
                    and ^T : (static member critical : string -> TraceTags.Tags -> unit)
                    and ^T : (static member verbose : string -> TraceTags.Tags -> unit)
                    and ^T : (static member event : string -> TraceTags.Tags -> unit)
                    and ^T : (static member flush : unit -> unit)
                    and ^T : (static member indent : unit -> unit)
                    and ^T : (static member unindent : unit -> unit)
                    and ^T : (static member trackException : System.Exception -> TraceTags.Tags -> unit)
                    /////
                > =
        {
            new ITagsTracer with
                member __.info message tags = (^T:(static member info : string -> TraceTags.Tags -> unit) (message, tags))
                member __.warning message tags = (^T:(static member warning : string -> TraceTags.Tags -> unit) (message, tags))
                member __.error message tags = (^T:(static member error : string -> TraceTags.Tags -> unit) (message, tags))
                member __.critical message tags = (^T:(static member critical : string -> TraceTags.Tags -> unit) (message, tags))
                member __.failwith message tags = (^T:(static member critical : string -> TraceTags.Tags -> unit) (message, tags)); failwith message 
                member __.verbose message tags = (^T:(static member verbose : string -> TraceTags.Tags -> unit) (message, tags))
                member __.event message tags = (^T:(static member event : string -> TraceTags.Tags -> unit) (message, tags))
                member __.writeLine message tags  = (^T:(static member writeLine : string -> TraceTags.Tags -> unit) (message, tags))
                member __.flush () = (^T:(static member flush : unit -> unit) ())
                member __.indent () =  (^T:(static member indent : unit -> unit) ())
                member __.unindent () = (^T:(static member unindent : unit -> unit) ())
                member __.trackException e tags = (^T:(static member trackException : System.Exception -> TraceTags.Tags -> unit) (e, tags))
        }