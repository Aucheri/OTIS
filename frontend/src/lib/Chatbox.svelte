<script>
	import { tick } from "svelte";
    import {marked} from "marked"

   
    let message = $state("")
    let messageHistory = $state([])
    let element = $state()
    let isSending = $state(false)

    async function SendData(event){
        if (isSending) return;

        if (message.trim() === "") return;
        
        isSending = true; 
        
        event.preventDefault()


        const m = message

        messageHistory.push(message)
        message = ""

        
        await tick()
        
        element.scroll({top: element.scrollHeight, behaviour: "smooth"});
        const response = await fetch("http://localhost:5156/message", {
            method: "POST",
            headers: {"Content-Type": "application/json"},
            
            body: JSON.stringify({Message:m, Messages:messageHistory.slice(0, -1)})
        })
        
        messageHistory = await response.json()

        await tick();

        element.scroll({top: element.scrollHeight, behaviour: "smooth"});

        isSending = false;
    }
</script>

<div class="grid grid-rows-[1fr_auto] h-screen w-screen p-10 gap-4">
    <div class="flex flex-col overflow-y-auto gap-2  overflow-x-hidden" bind:this={element}>
        {#each messageHistory as message, i}
            <span class={`${i % 2 ? "bg-neutral-600 after:bg-neutral-600 text-white rounded-bl-none" : "bg-neutral-200 after:bg-neutral-200 self-end rounded-br-none"} w-auto px-4 py-2 rounded-md max-w-3/4 wrap-anywhere relative after:block after:absolute after:right-0 after:bottom-0 after:translate-y-1/2 ${i % 2 ? "after:left-0 after:-translate-x-1/2" : "after:translate-x-1/2"} after:w-4 after:rotate-45 after:aspect-square after:`}>{@html marked(message)}</span>
        {/each}
    </div>
     
    <form onsubmit={SendData} class="flex gap-4">
        <input bind:value={message} class="w-3/4 rounded-lg" placeholder="Send a Chat"/>
        <button disabled={isSending} type="submit" class={`bg-neutral-500 px-4 flex-1 text-white rounded-lg cursor-pointer disabled:bg-neutral-400 disabled:cursor-not-allowed ${message.length > 0 ? "bg-blue-400! enabled:hover:bg-blue-500!" : ""} transition-colors duration-100 enabled:hover:bg-gray-600`}>Send Message</button>
    </form>
</div>