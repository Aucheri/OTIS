<script>
	import { marked } from "marked";
	import { tick } from "svelte";

   
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

<div class="grid grid-rows-[1fr_auto] h-full p-10 gap-4">
	
	{#if messageHistory.length <= 0}
		<div class="flex flex-col justify-center items-center">
			<h2 class="text-6xl font-bold">Chat to Otis</h2>
			<h3 class="text-3xl">Get help with your mental health!</h3>
		</div>
	{:else}
		<div class="flex flex-col overflow-y-auto gap-2  overflow-x-hidden" bind:this={element}>
			{#each messageHistory as message, i}
				<span class={`${i % 2 ? "bg-neutral-600 after:bg-neutral-600 text-white rounded-bl-none" : "bg-neutral-200 after:bg-neutral-200 self-end rounded-br-none"} w-fit px-4 py-2 rounded-md max-w-3/4 wrap-anywhere relative after:block after:absolute after:right-0 after:bottom-0 after:translate-y-1/2 ${i % 2 ? "after:left-0 after:-translate-x-1/2" : "after:translate-x-1/2"} after:w-4 after:rotate-45 after:aspect-square`}>{@html marked(message)}</span>
			{/each}
			
			{#if isSending}
				<span class={`bg-neutral-600 after:bg-neutral-600 w-fit px-4 py-2 rounded-md flex gap-2 max-w-3/4 relative after:block after:absolute after:right-0 after:bottom-0 after:translate-y-1/2 after:left-0 after:-translate-x-1/2 after:w-4 after:rotate-45 after:aspect-square`}>
					<div class="aspect-square w-3 rounded-full bg-neutral-200 animate-[bounce_1s_infinite_0ms]"></div>
					<div class="aspect-square w-3 rounded-full bg-neutral-200 animate-[bounce_1s_infinite_50ms]"></div>
					<div class="aspect-square w-3 rounded-full bg-neutral-200 animate-[bounce_1s_infinite_100ms]"></div>
				</span>
			{/if}
		</div>
	{/if}
		
     
    <form onsubmit={SendData} class="relative h-12 rounded-2xl left-1/2 -translate-x-1/2 bg-neutral-800 shadow-2xl">
        <input bind:value={message} class="w-full absolute h-full bg-transparent border-none outline-0 ring-0 text-white placeholder:text-neutral-300" placeholder="Ask for help with your mental health"/>
		<button type="submit" disabled={isSending || message.length <= 0} class="absolute h-4/5 translate-y-1/2 bottom-1/2 -translate-x-2 p-2 right-0 cursor-pointer bg-white rounded-full disabled:bg-neutral-500 disabled:cursor-not-allowed">
			<img src="up-arrow.svg" alt="Arrow Up" class="h-full z-10 aspect-square">
		</button>
    </form>
</div>